import { Injectable } from "@angular/core";
import { SignUpModel } from "../models/user";
import { UserModel } from "../models/auth";
import { HttpInternalService } from "./";

@Injectable({providedIn: 'root'})
export class UserService {
    private _routePrefix = '/api/user';

    constructor(private _httpService: HttpInternalService) { }

    public getUserFromToken() {
        return this._httpService.getFullRequest<UserModel>(`${this._routePrefix}/from-token`);
    }

    public signUp(user: SignUpModel) {
        return this._httpService.postFullRequest(
            `${this._routePrefix}/sign-up`, 
            user
        );
    }

    public copyUser({email, username, id }: UserModel) {
        return {
            email,
            username,
            id
        };
    }
}