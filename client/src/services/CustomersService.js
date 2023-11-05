import api from './AxiosService';

export const registerCustomer = async (registeredUser) => {
  return api.post('/api/customers/reg', registeredUser);
};

export const loginCustomer = async (loginUser) => {
  return api.post('/api/customers/login', loginUser);
};
