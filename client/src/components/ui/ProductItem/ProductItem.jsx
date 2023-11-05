import { Link, useNavigate } from 'react-router-dom';
import MainImageProduct from '../MainImageProduct/MainImageProduct';
import styles from './ProductItem.module.css';
import { useContext } from 'react';
import { ApplicationContext } from '../../../providers/ApplicationProvider';
import { addBasketItem } from '../../../services/BasketService';

const ProductItem = ({ product }) => {
  const nav = useNavigate();
  const { basket, setBasket } = useContext(ApplicationContext);

  return (
    <div className={styles.product}>
      <div className={styles.content}>
        <div
          className={styles.imageBlock}
          onClick={() => nav(`/product/${product.id}`)}
        >
          <MainImageProduct
            className={styles.image}
            imageId={product.imagesIds[0]}
            productName={product.productName}
          />
        </div>

        <div>
          <div className={styles.title}>{product.productName}</div>
          <div className={styles.description}>{product.productDescription}</div>
          <div className={styles.quantityProduct}>
            Осталось: {product.quantityInStoke} шт
          </div>
        </div>
      </div>

      <div className={styles.mainProductInfo}>
        <div className={styles.price}>{product.unitPrice} Р</div>
        {basket.find((bi) => bi.productId == product.id) ? (
          <Link
            className="button"
            style={{
              textAlign: 'center',
              textDecoration: 'none',
              fontSize: '15px',
              padding: '15px 24px',
              backgroundColor: '#10c44c',
              maxWidth: '180px',
              whiteSpace: 'nowrap',
            }}
            to="/basket"
          >
            Перейти в корзину
          </Link>
        ) : (
          <button
            className="button"
            style={{
              fontSize: '15px',
              padding: '15px 15px',
              maxWidth: '180px',
              whiteSpace: 'nowrap',
            }}
            onClick={async () => {
              const response = await addBasketItem(+product.id, 1);
              if (response.status == 200) {
                setBasket([...basket, response.data]);
              }
            }}
          >
            Добавить в корзину
          </button>
        )}
      </div>
    </div>
  );
};

export default ProductItem;
