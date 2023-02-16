import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  template: `
  <div class="page">
    <div class="loginholder">
      <h1>Login</h1>
      <div>
        <label>Email</label>
        <br>
        <input type="email" [(ngModel)]="email">
      </div>
      <br>
      <div>
        <label>Password</label>
        <br>
        <input type="password" [(ngModel)]="password">
      </div>
      <br>
      <button (click)="login()">Login</button>
      <hr>
      <h1>Don't have an account?</h1>
      <button  type="submit" [routerLink]="['/customer/register']">Sign up</button>
    </div>
  </div>

  {{ error }}
  `,
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  email: string = '';
  password: string = '';
  error = '';

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService) { }

  ngOnInit(): void {
    if (this.authService.CurrentUserValue != null && this.authService.CurrentUserValue.loginResponse.loginId > 0) {
      this.router.navigate(['/'])
    }
  }

  login(): void {
    this.error = '';
    this.authService.login(this.email, this.password).subscribe({
      next: () => {
        //get return url
        let returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.router.navigate([returnUrl]);
      },
      error: err => {
        if (err.error?.status == 400 || err.error?.status == 401 || err.error?.status == 500) {
          this.error = 'Forkert burgernavn eller kodeord';
        }
        else {
          this.error = err.error.title;
        }
      }
    });
  }

}
