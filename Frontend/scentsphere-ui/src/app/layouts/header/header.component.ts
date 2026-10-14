import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  imports: [CommonModule, RouterModule],
  selector: 'app-header',
  standalone: true,
  styleUrl: './header.component.scss',
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  showDropdown = false;
  constructor(
    private readonly router: Router,
    public readonly authService: AuthService,
  ) {}

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getUserName(): string {
    return localStorage.getItem('userName') || 'User';
  }

  logout(): void {
    this.authService.logout();

    this.showDropdown = false;

    this.router.navigate(['/login']);
  }
}
