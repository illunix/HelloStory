import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs/internal/Subject';
import { takeUntil } from 'rxjs/operators';
import { UserModel } from '@app/core/models/auth/user.model';
import { EventService } from '@app/core/services/event.service';
import { UserService } from '@app/core/services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {
  public authorizedUser: UserModel;
  private _unsubscribe$ = new Subject<void>();

  constructor(
    private _eventService: EventService,
    private _userService: UserService
  ) { }

  public ngOnInit(): void {
    this._eventService.userChangedEvent$
      .pipe(takeUntil(this._unsubscribe$))
      .subscribe(q => (this.authorizedUser = q ? this._userService.copyUser(q) : undefined));
  }

  public ngOnDestroy(): void {
    this._unsubscribe$.next();
    this._unsubscribe$.complete();
  }
}
