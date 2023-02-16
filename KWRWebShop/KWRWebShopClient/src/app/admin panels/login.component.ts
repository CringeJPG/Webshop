import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Login } from '../_models/login';
import { LoginService } from '../_services/login.service';
import {
  FormsModule,
  ReactiveFormsModule,
  FormGroup,
  Validator,
  FormControl,
  Validators,
} from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Role } from '../_models/role';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
  template: `
    <div class="FormHolder">
      <button routerLink="/admin/admin.dashboard" class="Dashboard">
        Dashboard
      </button>
      <h1>Login admin panel</h1>
      <form [formGroup]="loginForm" (ngSubmit)="save()">
        <div class="formControl">
          <label class="NavnLabel">Email: </label>
          <input class="NavnInput" type="text" formControlName="email" />
        </div>
        <div class="formControl">
          <label class="NavnLabel">Type: </label>
          <select class="CategoryInput" formControlName="type">
            <option value="0">0 - Customer</option>
            <option value="1">1 - Admin</option>
          </select>
        </div>
        <br />
        <br />
        <div class="CategoryButtonHolder">
          <button class="CategoryButtons" [disabled]="!loginForm.valid">
            Gem
          </button>
          <button class="CategoryButtons" type="button" (click)="cancel()">
            Annuller
          </button>
        </div>
      </form>
      <br />
      <br />

      <table class="CategoryTable">
        <tr>
          <th>Handling</th>
          <th>Id</th>
          <th>Email</th>
          <th>Role</th>
        </tr>

        <tr *ngFor="let login of logins">
          <td>
            <button class="CategoryTableBtn" (click)="edit(login)">Ret</button>
            <button class="CategoryTableBtn" (click)="delete(login)">
              Slet
            </button>
          </td>
          <td>{{ login.loginId }}</td>
          <td>{{ login.email }}</td>
          <td>{{ login.type }}</td>
        </tr>
      </table>
    </div>
  `,
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  logins: Login[] = [];
  login: Login = this.resetLogin();

  constructor(private loginService: LoginService) {}

  ngOnInit(): void {
    this.loginService.getAll().subscribe((x) => (this.logins = x));
  }

  edit(login: Login): void {
    console.log(login.loginId);
    Object.assign(this.login, login);
    this.loginForm.patchValue(login);
  }

  save(): void {
    if (this.loginForm.valid && this.loginForm.touched) {
      if (this.login.loginId == 0) {
        this.loginService.create(this.loginForm.value).subscribe({
          next: (x) => {
            this.logins.push(x);
            this.cancel();
          },
          error: (err) => {
            console.warn(Object.values(err.error.errors).join(', '));
          },
        });
      } else {
        this.loginService
          .update(this.login.loginId, this.loginForm.value)
          .subscribe({
            error: (err) => {
              console.warn(Object.values(err.error.errors).join(', '));
            },
            complete: () => {
              this.loginService.getAll().subscribe((x) => (this.logins = x));
              this.cancel();
            },
          });
      }
    }
  }

  delete(login: Login): void {
    if (confirm('Er du sikker på at du vil slette: ' + login.email + '?')) {
      this.loginService.delete(login.loginId).subscribe(() => {
        this.logins = this.logins.filter((l) => login.loginId != l.loginId);
      });
    }
  }

  resetLogin(): Login {
    console.log('reset');
    return { loginId: 0, email: '', password: '', type: Role.Admin };
  }

  cancel(): void {
    this.login = this.resetLogin();
    this.loginForm.reset();
  }

  loginForm: FormGroup = this.resetForm();

  resetForm(): FormGroup {
    return new FormGroup({
      email: new FormControl(null, Validators.required),
      role: new FormControl(null, Validators.required),
    });
  }
}
