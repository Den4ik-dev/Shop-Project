import api from './AxiosService';

export const addBasketItem = (productId, productCount) => {
  return api.post('api/customers/baskets', { productCount, productId });
};
