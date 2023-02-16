import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, first, map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Role } from '../_models/role';
import { Customer } from '../_models/customer';
import { Login } from '../_models/login';
import { EmailValidator } from '@angular/forms';
import { SignInResponse } from '../_models/signInResponse';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private currentUserSubject: BehaviorSubject<SignInResponse>;
    currentUser: Observable<SignInResponse>;

    constructor(private http: HttpClient) {
        // //fake login
        // if (sessionStorage.getItem('currentUser') == null) {
        //     sessionStorage.setItem('currentUser', JSON.stringify({ loginId: 1, email: '', type: Role.Admin }));
        // }
        this.currentUserSubject = new BehaviorSubject<SignInResponse>(
            JSON.parse(sessionStorage.getItem('currentUser') as string)
        );
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get CurrentUserValue(): SignInResponse {
        return this.currentUserSubject.value;
    }

    login(email: string, password: string) {
        let authenticateUrl = `${environment.apiUrl}Login/authenticate`;
        return this.http.post<SignInResponse>(authenticateUrl, { "email": email, "password": password }).pipe(map(user => {
            //store user details and token in local storage
            sessionStorage.setItem('currentUser', JSON.stringify(user));

            this.currentUserSubject.next(user as SignInResponse);
            return user;
        }))

    }
    logout() {
        //remove user from storage
        sessionStorage.removeItem('currentUser');
        //reset currentusersubject by fetching value in sessionstorage
        this.currentUserSubject = new BehaviorSubject<SignInResponse>(JSON.parse(sessionStorage.getItem('currentUser') as string));
        //reset currentuser to the reset usersubject
        this.currentUser = this.currentUserSubject.asObservable();
    }

    register(email: string, password: string, firstName: string, lastName: string, address: string): Observable<any> {
        let registerUrl = `${environment.apiUrl}Login/Register`;
        console.log(email, password, firstName, lastName, address);
        return this.http.post(registerUrl, { "email": email, "password": password, "customer": {"firstName": firstName, "lastName": lastName, "address": address} });
    }
}
