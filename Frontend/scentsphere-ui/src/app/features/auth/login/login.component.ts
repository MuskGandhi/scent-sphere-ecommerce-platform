import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { LoginRequest } from '../../../models/auth';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule],
  styleUrl: './login.component.scss',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  isLoading = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly notificationService: NotificationService,
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],

      password: ['', Validators.required],
    });
  }

  login(): void {
    if (this.loginForm.invalid) {
      return;
    }

    this.isLoading = true;

    const request = this.loginForm.value as LoginRequest;

    this.authService.login(request).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token);
        localStorage.setItem('role', response.role);
        localStorage.setItem('userName', response.userName);
        if (response.role === 'Admin') {
          this.router.navigate(['/admin/product-management']);
        } else {
          this.router.navigate(['/products']);
        }
      },

      error: () => {
        this.isLoading = false;

        this.notificationService.error('Invalid email or password');
      },
    });
  }
}
