import api from './AxiosService';

export const getProductRange = (limit, page) => {
  return api.get(`api/products?limit=${limit}&page=${page}`);
};
