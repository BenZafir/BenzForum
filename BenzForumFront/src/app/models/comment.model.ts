import { User } from "./user.model";

export interface PostComment {
  id: number;
  postId: number;
  content: string;
  userId: number;
  user: User;
}
