import axios from 'axios';

export const API_URL = 'https://localhost:8888';

const api = axios.create({
  withCredentials: true,
  baseURL: API_URL,
});

api.interceptors.request.use((config) => {
  config.headers.Authorization = `Bearer ${
    JSON.parse(localStorage.getItem('token'))
      ?.accessToken /* token: { accessToken: string, refreshToken: string } */
  }`;
  return config;
});

api.interceptors.response.use(
  (config) => {
    return config;
  },
  async (error) => {
    const originalRequest = error.config;
    console.log(error.response);
    if (error.response.status == 401 && !error.config._isRetry) {
      originalRequest._isRetry = true;

      try {
        const token = JSON.parse(localStorage.getItem('token'));

        const response = await api.post(`/api/token/refresh`, {
          accessToken: token?.accessToken,
          refreshToken: token?.refreshToken,
        });

        localStorage.setItem('token', JSON.stringify(response.data));

        return api.request(originalRequest);
      } catch (e) {
        console.log(e);
      }
    }
    throw error;
  }
);

export default api;
