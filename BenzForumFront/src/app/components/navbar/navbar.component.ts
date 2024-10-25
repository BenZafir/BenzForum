import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent implements OnInit, OnDestroy {
  isLoggedIn = false;
  constructor(private authService: AuthService) {
    this.isLoggedIn = this.checkAuthStatus();
  }
  subscription!: Subscription;

  ngOnInit(): void {
    this.subscription = this.authService.islogin.subscribe(
      (loginStatus: string) => {
        if (loginStatus === 'login') {
          this.isLoggedIn = true;
        } else if (loginStatus === 'logout') {
          this.isLoggedIn = false;
        }
      }
    );
  }
  logout() {
    this.isLoggedIn = false;
    this.authService.logout();
  }
  checkAuthStatus(): boolean {
    return this.authService.isAuthenticated();
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
