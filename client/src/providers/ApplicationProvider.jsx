import { createContext, useEffect, useState } from 'react';
import api from '../services/AxiosService';

export const ApplicationContext = createContext(/* default value */);

/* user :
  string firstName
  string lastName
  DateTime birthDate
  string phone
  string address
  string city
  int points
  int balance
  string email
  token: {
    string accessToken,
    string refreshToken
  }
*/
export const ApplicationProvider = ({ children }) => {
  const [user, setUser] = useState({ isLogin: false });
  const [basket, setBasket] = useState([]);

  useEffect(() => {
    if (localStorage.getItem('token')) {
      api.get('api/customers').then((response) => {
        setUser({ ...user, ...response.data, isLogin: true });
      });

      api.get('api/customers/baskets/all').then((response) => {
        setBasket([...response.data]);
      });
    }
  }, []);

  return (
    <ApplicationContext.Provider value={{ user, setUser, basket, setBasket }}>
      {children}
    </ApplicationContext.Provider>
  );
};
