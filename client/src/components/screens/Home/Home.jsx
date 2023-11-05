import Header from '../../ui/Header/Header';
import { useEffect, useRef, useState } from 'react';
import api from '../../../services/AxiosService';
import ProductMiniItem from '../../ui/ProductMiniItem/ProductMiniItem';

const Home = () => {
  const [products, setProducts] = useState([]);
  const [currentPage, setCurrentPage] = useState(0);
  const [fetching, setFetching] = useState(true);
  const totalCountRef = useRef(0);

  useEffect(() => {
    if (fetching) {
      api
        .get(`/api/products?limit=15&page=${currentPage}`)
        .then((response) => {
          setProducts([...products, ...response.data]);
          setCurrentPage(currentPage + 1);
          // setTotalCount(response.headers['x-total-count']);
          totalCountRef.current = response.headers['x-total-count'];
        })
        .finally(() => setFetching(false));
    }
  }, [fetching]);

  useEffect(() => {
    document.addEventListener('scroll', scrollHandler);

    return () => document.removeEventListener('scroll', scrollHandler);
  }, []);

  const scrollHandler = (e) => {
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
    <div className="container" style={{ justifyContent: 'center' }}>
      <Header />
      <div style={{ display: 'flex', flexWrap: 'wrap', margin: '0 auto' }}>
        {products.map((product) => (
          <ProductMiniItem product={product} key={product.id} />
        ))}
      </div>
    </div>
  );
};

export default Home;
