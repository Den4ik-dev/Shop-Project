import { useEffect, useState } from 'react';
import api from '../../../services/AxiosService';
import styles from './Comment.module.css';

const AuthorComment = ({ customerId }) => {
  const [author, setAuthor] = useState();

  useEffect(() => {
    api
      .get(`/api/customers/${customerId}`)
      .then((response) => setAuthor(response.data));
  }, []);

  return <div className={styles.author}>{author}</div>;
};

export default AuthorComment;
