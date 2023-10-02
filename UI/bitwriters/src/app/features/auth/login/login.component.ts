import { Component } from '@angular/core';
import { LoginRequest } from '../models/login-request.model';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  model: LoginRequest;

  constructor(private authService: AuthService) {
    this.model = {
      email: '',
      password: ''
    };
  }

  onFormSubmit(): void {
    this.authService.login(this.model).subscribe({
      next: (response)=>{
        console.log(response);
      }
    })
  }
}
