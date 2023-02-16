import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Customer } from '../_models/customer';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  template: `
   <form (ngSubmit)="register()">
    <div class="formControl">
        <label>Email</label>
        <br>
        <input type="text" [(ngModel)]="form.email" name="email">
    </div>
    <br>
    <div class="formControl">
        <label>password</label>
        <br>
        <input type="password" [(ngModel)]="form.password" name="password">
    </div>
    <br>
    <div class="formControl">
        <label>firstName</label>
        <br>
        <input type="text" [(ngModel)]="form.firstName" name="firstName">
    </div>
    <br>
    <div class="formControl">
        <label>lastName</label>
        <br>
        <input type="text" [(ngModel)]="form.lastName" name="lastName">
    </div>
    <br>
    <div class="formControl">
        <label>address</label>
        <br>
        <input type="text" [(ngModel)]="form.address" name="address">
    </div>
    <button>Sign up</button>
</form>
  `,
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  form: any = {
    email: null,
    password: null,
    firstName: null,
    lastName: null,
    address: null
  };
  error = '';

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService) { }

  ngOnInit(): void {
  }

  register(): void {
    this.error = '';
    const { email, password, firstName, lastName, address } = this.form
    this.authService.register(email, password, firstName, lastName, address).subscribe({
      next: () => {
        console.log(email, password, firstName, lastName, address);
        let returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.router.navigate([returnUrl]);
      },
      error: (x) => {
        console.log()
        console.log(email, password, firstName, lastName, address);
      }
    })
  }
}
