import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormGroup,
  FormControl,
  Validators,
  FormsModule,
} from '@angular/forms';
import { Customer } from '../_models/customer';
import { CustomerService } from '../_services/customer.service';
import { RouterModule } from '@angular/router';
import { Login } from '../_models/login'

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, FormsModule],
  template: `
    <div class="FormHolder">
      <button routerLink="/admin/admin.dashboard" class="Dashboard">
        Dashboard
      </button>
      <h1>Customer Administration</h1>
      <form [formGroup]="userForm"(ngSubmit)="save()">
        <div formGroupName="login">
          <label class="NavnLabel">Email: </label>
          <input class="emailInput" type="text" formControlName="email" />
          <span
            class="error"
            *ngIf="userForm.get('email')?.invalid && userForm.get('email')?.touched">Needs a email</span>
          <br />
          <div class="formControl">
           <label class="NavnLabel">Password: </label>
           <input class="passwordInput" type="text" formControlName="password" />
          </div>
          <br>
          <div class="formControl">
          <label class="NavnLabel">Type: </label>
          <select class="CategoryInput" formControlName="type">
            <option value="Admin">0 - Admin</option>
            <option value="User">1 - Customer</option>
          </select>
        </div>
        </div>
        <br />
        <div class="formControl">
          <label class="NavnLabel">First name: </label>
          <input
            class="FirstNameInput"
            type="text"
            formControlName="firstName"
          />
          <span
            class="error"
            *ngIf="
              userForm.get('firstName')?.invalid &&
              userForm.get('firstName')?.touched
            "
            >First name required</span
          >
        </div>
        <br />
        <div class="formControl">
          <label class="NavnLabel">Last name: </label>
          <input class="LastNameInput" type="text" formControlName="lastName" />
          <span
            class="error"
            *ngIf="
              userForm.get('lastName')?.invalid &&
              userForm.get('lastName')?.touched
            "
            >Last name required</span
          >
        </div>
        <br />
        <div class="formControl">
          <label class="NavnLabel">Address: </label>
          <input class="AddressInput" type="text" formControlName="address" />
          <span
            class="error"
            *ngIf="
              userForm.get('address')?.invalid &&
              userForm.get('address')?.touched
            "
            >User needs an address</span
          >
        </div>
        <button class="userButtons" [disabled]="!userForm.valid">Gem</button>
        <button class="userButtons" type="button" (click)="Cancel()">
          Annuller
        </button>
      </form>

      <table class="userTable">
        <tr>
          <th>Handling</th>
          <th>Customer Id</th>
          <th>Login Id</th>
          <th>Email</th>
          <th>First name</th>
          <th>Last name</th>
          <th>Address</th>
          <th>Created</th>
        </tr>
        <tr *ngFor="let user of users">
          <td>
            <button class="usersTableBtn" (click)="edit(user)">Ret</button>
            <button class="usersTableBtn" (click)="deleteUser(user)">
              Slet
            </button>
          </td>
          <td>{{ user.customerId }}</td>
          <td>{{ user.login.loginId }}</td>
          <td>{{ user.login.email }}</td>
          <td>{{ user.firstName }}</td>
          <td>{{ user.lastName }}</td>
          <td>{{ user.address }}</td>
          <td>{{ user.created }}</td>
        </tr>
      </table>
    </div>
  `,
  styleUrls: ['./user.component.css'],
})
export class UserComponent implements OnInit {
  users: Customer[] = [];
  user: Customer = this.resetUser();
  errors: string = '';
  userForm: FormGroup = this.resetForm();

  constructor(private customerService: CustomerService) { }

  resetForm(): FormGroup {
    return new FormGroup({
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      address: new FormControl(null, Validators.required),
      login: new FormGroup({
        email: new FormControl(null, Validators.required),
        password: new FormControl(null, Validators.required),
        type: new FormControl(null)
      })
    });

  }

  resetUser(){
    return {
      customerId: 0, loginId: 0, firstName: '', lastName: '', address: '', created: new Date, login: { loginId: 0, email: '', password: '', type: undefined}
    }
  }
  ngOnInit(): void {
    this.customerService.getAll().subscribe((x) => (this.users = x));
  }
  save(): void {
    if (this.userForm.valid && this.userForm.touched) {
      if (this.user.customerId == 0) {

        console.log(this.userForm.value)
        this.customerService.create(this.userForm.value).subscribe({
          next: (x) => {
            console.log(x)
            this.users.push(x);
            this.Cancel();
          },
          error: (err) => {
            console.warn(Object.values(err.error.errors).join(', '));
            this.errors = Object.values(err.error.errors).join(', ');
          },
        });
      } else {
        this.customerService
          .update(this.user.customerId, this.userForm.value)
          .subscribe({
            error: (err) => {
              console.warn(Object.values(err.error.errors).join(', '));
              this.errors = Object.values(err.error.errors).join(', ');
            },
            complete: () => {
              this.customerService.getAll().subscribe((x) => (this.users = x));
              this.Cancel();
            },
          });
        }
      }
    
  }
  edit(user: Customer): void {
    user.loginId = user.login.loginId;
    Object.assign(this.user, user); //Copies the values of the hero u click on and pastes it in the input boxes
    this.userForm.patchValue(user);
  }
  Cancel(): void {
    this.user = this.resetUser();
    this.errors = '';
    this.userForm = this.resetForm();
  }
  deleteUser(user: Customer): void {
    if (confirm('Er du sikker på du vil slette' + user.customerId + '?')) {
      this.customerService.delete(user.customerId).subscribe((x) => {
        this.users = this.users.filter((o) => user.customerId != o.customerId);
      });
    }
  }
}
