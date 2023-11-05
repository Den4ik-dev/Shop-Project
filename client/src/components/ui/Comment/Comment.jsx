import AuthorComment from './AuthorComment';
import styles from './Comment.module.css';

const Comment = ({ comment }) => {
  return (
    <div className={styles.comment}>
      <AuthorComment customerId={comment.customerId} />
      <div>{comment.content}</div>
    </div>
  );
};

export default Comment;
