import { useEffect, useRef, useState } from 'react';
import api from '../../../services/AxiosService';
import Comment from '../Comment/Comment';
import CommentsInput from '../CommentsInput/CommentsInput';

const Comments = ({ productId }) => {
  const [comments, setComments] = useState([]);
  const [currentPage, setCurrentPage] = useState(0);
  const [fetching, setFetching] = useState(true);
  const totalCountRef = useRef(0);

  useEffect(() => {
    if (fetching) {
      api
        .get(
          `/api/products/comments?productId=${productId}&limit=1&page=${currentPage}`
        )
        .then(async (response) => {
          setComments([...comments, ...response.data]);
          setCurrentPage(currentPage + 1);

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
      comments.length < /*totalCount*/ totalCountRef.current
    ) {
      setFetching(true);
    }
  };

  const addComment = (comment) => {
    setComments([comment, ...comments]);
    setCurrentPage(currentPage + 1);
  };

  return (
    <div style={{ padding: '100px 0 100px 0', borderTop: '2px solid #0080ff' }}>
      <div>Отзывы о товаре</div>

      <CommentsInput productId={productId} addComment={addComment} />

      <div>
        {comments.map((comment) => (
          <Comment key={comment.id} comment={comment} />
        ))}
      </div>
    </div>
  );
};

export default Comments;
