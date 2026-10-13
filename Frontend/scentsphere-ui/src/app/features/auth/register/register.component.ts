import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { Router, RouterModule } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';
import { RegisterRequest } from '../../../models/auth';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;

  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],

      lastName: ['', Validators.required],

      email: ['', [Validators.required, Validators.email]],

      phoneNumber: ['', Validators.required],

      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  register(): void {
    if (this.registerForm.invalid) {
      console.log('Form Invalid', this.registerForm.errors, this.registerForm.value);
      this.registerForm.markAllAsTouched();

      return;
    }

    console.log('Sending Request', this.registerForm.value);

    this.isLoading = true;

    const request = this.registerForm.value as RegisterRequest;

    this.authService.register(request).subscribe({
      next: (response) => {
        this.isLoading = false;

        alert('Registration Successful');

        this.router.navigate(['/login']);
      },

      error: (error) => {
        console.error('Registration Error', error);

        this.isLoading = false;
      },
    });
  }
}
