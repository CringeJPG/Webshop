import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Customer } from '../_models/customer';
import { CustomerService } from '../_services/customer.service';
import { UserpageComponent } from './userpage.component';

@Component({
  selector: 'app-useredit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `

  `,
  styles: [
  ]
})
export class UsereditComponent implements OnInit {

  ngOnInit(): void {}
    

}

