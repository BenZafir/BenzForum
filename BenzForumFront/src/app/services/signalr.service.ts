import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { Post } from '../models/post.model';
import { PostComment } from '../models/comment.model';
@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;
  public postReceived = new Subject<Post>();
  public commentReceived = new Subject<PostComment>();
  public postListUpdate = new Subject<string>();
  private apiUrl = environment.apiUrl;

  constructor() {
    this.startConnection();
    this.registerOnServerEvents();
  }

  // Start SignalR connection
  private startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.apiUrl}/commentHub`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      }) // Update with your backend SignalR hub URL
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connected'))
      .catch((err) => {
        console.error('Error starting SignalR connection:', err);
        this.handleConnectionError(err);
      });
  }

  // Register server events
  private registerOnServerEvents(): void {
    this.hubConnection.on('ReceivePost', (post: any) => {
      this.postReceived.next(post);
    });

    this.hubConnection.on('ReceiveComment', (comment: any) => {
      this.commentReceived.next(comment);
    });

    this.hubConnection.on('PostListUpdate', (note:string) => {
      this.postListUpdate.next(note);
    });
  }

  // Handle connection errors
  private handleConnectionError(error: any): void {
    if (error instanceof signalR.HttpError) {
      console.error('HTTP error:', error);
    } else if (error instanceof signalR.TimeoutError) {
      console.error('Timeout error:', error);
    } else {
      console.error('Unknown error:', error);
    }
  }
}