import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ForumService } from '../../services/forum.service';
import { Post } from '../../models/post.model';
import { PostComment } from '../../models/comment.model';
import { SignalRService } from '../../services/signalr.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RichTextComponent } from '../rich-text/rich-text.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-post-detail',
  imports: [CommonModule, FormsModule, RichTextComponent],
  standalone: true,
  templateUrl: './post-detail.component.html',
  styleUrl: './post-detail.component.scss',
})
export class PostDetailComponent implements OnInit, OnDestroy {
  post!: Post;
  newCommentContent = signal<string>('');
  commentReceivedSubscription!: Subscription;
  postListUpdateSubscription!: Subscription;

  constructor(
    private route: ActivatedRoute,
    private forumService: ForumService,
    private signalRService: SignalRService
  ) {}

  ngOnInit(): void {
    const postId = Number(this.route.snapshot.paramMap.get('id'));
    this.getPost(postId);
    // Subscribe to new comments
    this.commentReceivedSubscription =
      this.signalRService.commentReceived.subscribe((comment: PostComment) => {
        if (comment.postId === this.post.id) {
          this.post.comments.push(comment);
        }
      });
    this.postListUpdateSubscription =
      this.signalRService.postListUpdate.subscribe((update: string) => {
        this.getPost(postId);
      });
  }

  deleteComment(commentId: number): void {
    this.forumService.deleteComment(commentId).subscribe({
      next: (response) => {
        // Reload the post to update the comments
        const postId = Number(this.route.snapshot.paramMap.get('id'));
        this.getPost(postId);
      },
      error: (error) => {
        console.error('Error deleting comment', error);
        alert(error.error.message);
      },
    });
  }
  // Fetch post details
  getPost(postId: number): void {
    this.forumService.getPost(postId).subscribe({
      next: (data: Post) => {
        this.post = data;
        this.getComments(postId);
      },
      error: (error) => {
        console.error('Error fetching post:', error);
      },
    });
  }
  getComments(postId: number): void {
    this.forumService.getComments(postId).subscribe({
      next: (data: PostComment[]) => {
        this.post.comments = data;
      },
      error: (error) => {
        console.error('Error fetching Comments:', error);
      },
    });
  }

  // Add a new comment
  addComment(): void {
    const commentData = {
      content: this.newCommentContent(),
      postId: this.post.id,
    };
    this.forumService.addComment(commentData).subscribe({
      next: (data: PostComment) => {
        this.getPost(this.post.id);
        this.newCommentContent.set('');
      },
      error: (error) => {
        console.error('Error adding comment:', error);
      },
    });
  }
  ngOnDestroy(): void {
    this.commentReceivedSubscription.unsubscribe();
    this.postListUpdateSubscription.unsubscribe();
  }
}
