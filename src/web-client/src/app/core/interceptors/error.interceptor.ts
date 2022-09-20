import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "@app/core/services/auth.service";
import { Observable, throwError } from "rxjs";
import { catchError, switchMap } from "rxjs/operators";
import { ErrorCodes } from "../enums/common/error-codes.enum";


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(
        private _router: Router, 
        private _authService: AuthService
    ) {}

    public intercept(
        req: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError((response) => {
                if (response.status === 401) {
                    if (response.headers.has('Token-Expired')) {
                        return this._authService.refreshAccessToken().pipe(
                            switchMap((resp) => {
                                req = req.clone({
                                    setHeaders: {
                                        Authorization: `Bearer ${resp.accessToken}`
                                    }
                                });

                                return next.handle(req);
                            })
                        );
                    }

                    if (response.error) {
                        if (
                            response.error.code === ErrorCodes.InvalidToken && 
                            !this._authService.areAccessTokensExist()
                        ) {
                            return throwError(response.error.error);
                        }
                        if (response.error.code === ErrorCodes.ExpiredRefreshToken) {
                            this._router.navigate(['/']);
                            this._authService.signOut();
                            return throwError(response.error.error);
                        }
                    }
                }

                const error = response.error
                    ? response.error || response.error.message
                    : response.message || `${response.status} ${response.statusText}`;

                return throwError(error);
            })
        );
    }
}
