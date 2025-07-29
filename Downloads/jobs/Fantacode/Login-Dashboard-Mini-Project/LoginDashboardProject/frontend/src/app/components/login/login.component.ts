import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { LoginRequest } from '../../models/login.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginModel: LoginRequest = {
    username: '',
    password: ''
  };
  errorMessage: string | null = null;
  isLoading = false;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    // Redirect to dashboard if already logged in
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
    }
  }

  onSubmit(): void {
    this.isLoading = true;
    this.errorMessage = null;
    
    console.log('Submitting login form with username:', this.loginModel.username);
    
    this.authService.login(this.loginModel).subscribe({
      next: (response) => {
        console.log('Login successful, response:', response);
        this.authService.setToken(response.token);
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Login error details:', error);
        
        if (error.status === 0) {
          this.errorMessage = 'Cannot connect to the server. Please check your network connection.';
        } else if (error.status === 401) {
          this.errorMessage = 'Invalid username or password. Please try again.';
        } else {
          this.errorMessage = `Login failed: ${error.message || 'Unknown error'}`;
        }
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }
}