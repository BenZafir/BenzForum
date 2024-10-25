import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Post } from '../models/post.model';
import { PostComment } from '../models/comment.model';

@Injectable({
  providedIn: 'root',
})
export class ForumService {
  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  // Get all posts
  getPosts(): Observable<Post[]> {
    return this.http.get<Post[]>(`${this.apiUrl}/Posts`);
  }

  // Get a single post by ID
  getPost(id: number): Observable<Post> {
    return this.http.get<Post>(`${this.apiUrl}/posts/${id}`);
  }

  // Get a single post by ID
  getComments(postId: number): Observable<PostComment[]> {
    return this.http.get<PostComment[]>(`${this.apiUrl}/Comments/Post/${postId}`);
  }

  // Create a new post
  createPost(postData: any): Observable<Post> {
    return this.http.post<Post>(`${this.apiUrl}/Posts`, postData);
  }

  // Add a comment to a post
  addComment(commentData: any): Observable<PostComment> {
    return this.http.post<PostComment>(`${this.apiUrl}/Comments`, commentData);
  }
  deletePost(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Posts/${id}`);
  }
  deleteComment(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Comments/${id}`);
  }
}
