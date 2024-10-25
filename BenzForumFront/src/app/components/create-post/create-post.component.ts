import { Component } from '@angular/core';
import { ForumService } from '../../services/forum.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RichTextComponent } from '../rich-text/rich-text.component';
import { BasePost } from '../../models/basePost.model';

@Component({
  selector: 'app-create-post',
  imports: [FormsModule,RichTextComponent],
  standalone: true,
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent {
  post:BasePost = {
    title: '',
    content: ''
  };

  constructor(private forumService: ForumService, private router: Router) {}

  onSubmit(): void {
    if (this.post.title && this.post.content) {
      this.forumService.createPost(this.post).subscribe({
        next: response => {
          // Navigate to the /forum route after successful submission
          this.router.navigate(['/forum']);
        },
        error: error => {
          console.error('Error creating post', error);
        }
      });
    } else {
      console.error('Form is invalid');
    }
  }
  onDelete(): void {
    // Optionally, reset the form or navigate to another page
    this.post = {
      title: '',
      content: ''
    };
  }
}
