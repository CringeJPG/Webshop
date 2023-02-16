import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Customer } from '../_models/customer';
import { CustomerService } from '../_services/customer.service';
import { Login } from '../_models/login';
import { AuthService } from '../_services/auth.service';
import { OrderService } from '../_services/order.service';
import { Order } from '../_models/order';

@Component({
  selector: 'app-userpage',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <h3>Velkommen {{this.currentUser.loginResponse.customer.firstName}} {{this.currentUser.loginResponse.customer.lastName}}</h3>
    <p>Address: {{this.currentUser.loginResponse.customer.address}}</p>
    <div>
  `,
  styleUrls: ['./userpage.compontent.css'],
})
export class UserpageComponent implements OnInit {
  currentUser: any = {}
  constructor(private authService: AuthService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.authService.currentUser.subscribe(x => {
      this.currentUser = x
    })

  }

}
