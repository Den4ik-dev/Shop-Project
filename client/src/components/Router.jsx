import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Home from './screens/Home/Home';
import RegisterForm from './screens/RegisterForm/RegisterForm';
import LoginForm from './screens/LoginForm/LoginForm';
import Account from './screens/Account/Account';
import ProductDetail from './screens/ProductDetail/ProductDetail';
import Basket from './screens/Basket/Basket';
import SearchByTextPage from './screens/SearchByTextPage/SearchByTextPage';
import { useQuery } from '../hooks/useQuery';

const Router = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Home />} path="/" />
        <Route element={<RegisterForm />} path="/reg" />
        <Route element={<LoginForm />} path="/login" />
        <Route element={<Account />} path="/account" />
        <Route element={<ProductDetail />} path="/product/:id" />
        <Route element={<Basket />} path="/basket" />
        <Route element={<SearchByTextPage />} path="/search" />
      </Routes>
    </BrowserRouter>
  );
};

export default Router;
