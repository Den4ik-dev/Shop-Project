import { useState } from 'react';
import { useEffect } from 'react';
import styles from './ProductItem.module.css';
import api from '../../../services/AxiosService';
import { useNavigate } from 'react-router-dom';
import MainImageProduct from '../MainImageProduct/MainImageProduct';

/* 
  product {
    int id
    string productName
    string productDescription
    int unitPrice
    int quantityInStoke
    int categoryId 
    int manufacturerId
    IEnumerable<int> imagesIds
    IEnumerable<int> videosIds
  }
*/
const ProductMiniItem = ({ product }) => {
  const nav = useNavigate();

  return (
    <div
      className={styles.product}
      onClick={() => nav(`/product/${product.id}`)}
    >
      <div style={{ flex: '1 1 auto' }}>
        <MainImageProduct
          className={styles.image}
          imageId={product.imagesIds[0]}
          productName={product.productName}
        />
      </div>

      <div className={styles.content}>
        <div className={styles.price}>{product.unitPrice} Р</div>
        <div className={styles.title}>{product.productName}</div>
        <div className={styles.description}>{product.productDescription}</div>
        <div className={styles.quantityProduct}>
          Осталось: {product.quantityInStoke} шт
        </div>
      </div>
    </div>
  );
};

export default ProductMiniItem;
