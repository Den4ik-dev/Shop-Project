import { useEffect, useState } from 'react';
import api from '../../../services/AxiosService';
import styles from './MainImageProduct.module.css';

const MainImageProduct = ({ imageId, productName }) => {
  const [imagePath, setImagePath] = useState();

  useEffect(() => {
    api
      .get(`/api/products/images/${imageId}`)
      .then((response) => setImagePath(response.data));
  }, [imageId]);

  return (
    <img
      className={styles.image}
      src={imagePath}
      alt={`Изображение товара ${productName}`}
    />
  );
};

export default MainImageProduct;
