import { Injectable } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { map } from 'rxjs/operators';
import { HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { UserModel } from '../models/auth/user.model';
import { UserService } from './user.service';
import { EventService } from './event.service';
import { SignInModel } from '../models/auth/sign-in.model';
import { AuthResponseModel } from '../models/auth/auth-response.model';
import { AccessTokenModel } from '../models/auth/access-token.model';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private _routePrefix = '/api/authflow';
    private _user: UserModel;

    constructor(
        private _httpService: HttpInternalService,
        private _userService: UserService,
        private _eventService: EventService
    ) { }

    public getUser() {
        return this._user
            ? of(this._user)
            : this._userService.getUserFromToken().pipe(
                map((resp) => {
                    this._user = resp.body;
                    this._eventService.userChanged(this._user);
                    return this._user;
                })
            );
    }

    public signIn(user: SignInModel) {
        return this.handleAuthResponse(this._httpService.postFullRequest(
            `${this._routePrefix}/sign-in`, 
            user
        ));
    }

    public signOut() {
        this.revokeRefreshToken();
        localStorage.removeItem('accessToken');
        this._user = undefined;
        this._eventService.userChanged(undefined);
    }

    public refreshAccessToken() {
        return this._httpService
            .postFullRequest<AccessTokenModel>(
                `${this._routePrefix}/token/refresh`,
                { accessToken: JSON.parse(localStorage.getItem('accessToken')) }
            )
            .pipe(
                map((resp) => {
                    this.setAccessToken(resp.body.accessToken);
                    return resp.body;
                })
            );
    }

    public revokeRefreshToken() {
        return this._httpService.postFullRequest(
            `${this._routePrefix}/token/revoke`,
            { accessToken: JSON.parse(localStorage.getItem('accessToken')) }
        );
    }

    private handleAuthResponse(observable: Observable<HttpResponse<AuthResponseModel>>) {
        return observable.pipe(
            map((resp) => {
                this.setAccessToken(resp.body.accessToken);
                this._user = resp.body.user;
                this._eventService.userChanged(resp.body.user);
                return resp.body.user;
            })
        );
    }

    private setAccessToken(accessToken: string) {
        if (accessToken) {
            localStorage.setItem(
                'accessToken',
                JSON.stringify(accessToken)
            );
            this.getUser();
        }
    }
}
