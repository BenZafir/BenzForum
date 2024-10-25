import {
  Component,
  OnDestroy,
  OnInit,
  signal,
  WritableSignal,
} from '@angular/core';
import { ForumService } from '../../services/forum.service';
import { Post } from '../../models/post.model';
import { Router } from '@angular/router';
import { SignalRService } from '../../services/signalr.service';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-forum',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './forum.component.html',
  styleUrl: './forum.component.scss',
})
export class ForumComponent implements OnInit, OnDestroy {
  posts: Post[] = [];

  constructor(
    private forumService: ForumService,
    private router: Router,
    private signalRService: SignalRService
  ) {}

  postReceivedSubscription!: Subscription;
  postListUpdateSubscription!: Subscription;
  ngOnInit(): void {
    this.loadPosts();
    this.postReceivedSubscription = this.signalRService.postReceived.subscribe(
      (post: Post) => {
        this.posts.push(post);
      }
    );
    this.postListUpdateSubscription =
      this.signalRService.postListUpdate.subscribe((update: string) => {
        this.loadPosts();
      });
  }

  // Load all posts
  loadPosts(): void {
    this.forumService.getPosts().subscribe({
      next: (data: Post[]) => {
        this.posts = data;
      },
      error: (error) => {
        console.error('Error fetching posts:', error);
      },
    });
  }

  // Navigate to post details
  viewPost(postId: number): void {
    this.router.navigate(['/post', postId]);
  }
  // Navigate to post details
  viewCreatePost(): void {
    this.router.navigate(['/create-post']);
  }

  deletePost(postId: number): void {
    this.forumService.deletePost(postId).subscribe({
      next: (response) => {
        this.loadPosts();
      },
      error: (error) => {
        console.error('Error deleting post', error);
        alert(error.error.message);
      },
    });
  }
  ngOnDestroy(): void {
    this.postReceivedSubscription.unsubscribe();
    this.postListUpdateSubscription.unsubscribe();
  }
}
