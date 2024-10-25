import { BasePost } from './basePost.model';
import { PostComment } from './comment.model';
import { User } from './user.model';

export interface Post extends BasePost {
  id: number;
  userId: number;
  user: User;
  comments: PostComment[];
}
