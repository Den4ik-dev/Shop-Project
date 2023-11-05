import { useQuery } from '../../../hooks/useQuery';
import Header from './../../ui/Header/Header';
import { useEffect, useRef, useState } from 'react';
import api from '../../../services/AxiosService';
import ProductItem from '../../ui/ProductItem/ProductItem';

const SearchByTextPage = () => {
  const text = useQuery().get('text');
  const [products, setProducts] = useState([]);
  const [currentPage, setCurrentPage] = useState(0);
  const [fetching, setFetching] = useState(true);
  const totalCountRef = useRef(0);

  useEffect(() => {
    console.log('fetching is true');
    setCurrentPage(0);
    setProducts([]);
    setFetching(true);
  }, [text]);

  useEffect(() => {
    if (fetching) {
      api
        .get(`/api/products/input?limit=10&page=${currentPage}&text=${text}`)
        .then((response) => {
          setProducts([...products, ...response.data]);
          setCurrentPage(currentPage + 1);
          console.log(response.data); /* CLEAR */
          totalCountRef.current = response.headers['x-total-count'];
        })
        .finally(() => setFetching(false));
    }
  }, [fetching]);

  useEffect(() => {
    console.log('useEffect');
    document.addEventListener('scroll', scrollHandler);

    return () => {
      console.log('remove useEffect');
      document.removeEventListener('scroll', scrollHandler);
    };
  }, [text]);

  const scrollHandler = (e) => {
    console.log(totalCountRef.current);
    if (
      e.target.documentElement.scrollHeight -
        (e.target.documentElement.scrollTop + window.innerHeight) <
        100 &&
      products.length < /*totalCount*/ totalCountRef.current
    ) {
      setFetching(true);
    }
  };

  return (
    <div className="container">
      <Header initInputValue={text} />
      <div>
        <div style={{ margin: '10px 0 30px 0', fontWeight: '700' }}>
          {totalCountRef.current == 0 ? (
            <>По запросу {text} не найдено товаров</>
          ) : totalCountRef.current > 1 ? (
            <>
              По запросу {text} найдено {totalCountRef.current} товара
            </>
          ) : (
            <>
              По запросу {text} найден {totalCountRef.current} товар
            </>
          )}
        </div>
        <div>
          {products.map((product) => (
            <ProductItem key={product.id} product={product} />
          ))}
        </div>
      </div>
    </div>
  );
};

export default SearchByTextPage;
