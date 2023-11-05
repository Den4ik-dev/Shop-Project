import { useEffect, useState } from 'react';
import api from '../../../services/AxiosService';
import MainImageProduct from '../MainImageProduct/MainImageProduct';
import BasketItemCounter from './BasketItemCounter';
import styles from './BasketItem.module.css';
import { useNavigate } from 'react-router-dom';

const BasketItem = ({
  basketItem,
  changeBasketItemCountProduct,
  removeBasketItem,
}) => {
  const [productInfo, setProductInfo] = useState();
  const nav = useNavigate();

  useEffect(() => {
    api
      .get(`api/products/${basketItem.productId}`)
      .then((response) => setProductInfo(response.data));
  }, []);

  return (
    <div className={styles.basketItem}>
      <div style={{ display: 'flex' }}>
        <div className={styles.image}>
          <MainImageProduct
            imageId={productInfo?.imagesIds?.[0]}
            productName={productInfo?.productName}
          />
        </div>
        <div className={styles.productInfo}>
          <div
            className={styles.productName}
            onClick={() => nav(`/product/${basketItem.productId}`)}
          >
            {productInfo?.productName}
          </div>
          <div className={styles.productDescription}>
            {productInfo?.productDescription}
          </div>
          <div style={{ display: 'flex' }}>
            <button
              className={styles.deleteButton}
              onClick={() => {
                removeBasketItem(basketItem.id);
              }}
            >
              <img src="basketItem/delete-icon.svg" alt="remove" />
            </button>
          </div>
        </div>
      </div>
      <div>
        <div className={`price ${styles.price}`}>
          {productInfo?.unitPrice} Р
        </div>
      </div>
      <div>
        <BasketItemCounter
          basketItem={basketItem}
          changeBasketItemCountProduct={changeBasketItemCountProduct}
        />
      </div>
    </div>
  );
};

export default BasketItem;
