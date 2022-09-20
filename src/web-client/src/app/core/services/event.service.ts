import { Injectable } from "@angular/core";
import { UserModel } from "@app/core/models/auth/user.model";
import { Subject } from "rxjs";

@Injectable({ providedIn: 'root' })
export class EventService {
    private onUserChanged = new Subject<UserModel>();
    public userChangedEvent$ = this.onUserChanged.asObservable();

    public userChanged(user: UserModel) {
        this.onUserChanged.next(user);
    }
}