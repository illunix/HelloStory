import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class HttpInternalService {
    private _baseUrl: string = environment.apiUrl;
    private _headers = new HttpHeaders();

    constructor(private _http: HttpClient) { }

    public getHeaders(): HttpHeaders {
        return this._headers;
    }

    public getRequest<T>(
        url: string,
        httpParams?: any
    ): Observable<T> {
        return this._http.get<T>(
            this.buildUrl(url), 
            {
                 headers: this.getHeaders(),
                  params: httpParams 
            }
        );
    }

    public getFullRequest<T>(
        url: string, 
        httpParams?: any
    ): Observable<HttpResponse<T>> {
        return this._http.get<T>(
            this.buildUrl(url), 
            {
                observe: 'response', 
                headers: this.getHeaders(), 
                params: httpParams
            }
        );
    }

    public DownloadRequest(
        url: string,
        httpParams?: any
    ) {
        return this._http.get(
            this.buildUrl(url), 
            { 
                observe: 'response', 
                responseType: 'blob', 
                headers: this.getHeaders(), 
                params: httpParams 
            }
        );
    }

    public postClearRequest<T>(
        url: string, 
        payload: object
    ): Observable<T> {
        return this._http.post<T>(this.buildUrl(url), payload);
    }

    public postRequest<T>(
        url: string, 
        payload: object
    ): Observable<T> {
        return this._http.post<T>(
            this.buildUrl(url),
            payload, 
            { headers: this.getHeaders() }
        );
    }

    public postFullRequest<T>(
        url: string, 
        payload: object
    ): Observable<HttpResponse<T>> {
        return this._http.post<T>(
            this.buildUrl(url), 
            payload, 
            { 
                headers: this.getHeaders(), 
                observe: 'response' 
            }
        );
    }

    public putRequest<T>(
        url: string,
         payload: object
    ): Observable<T> {
        return this._http.put<T>(
            this.buildUrl(url),
            payload, 
            { headers: this.getHeaders() }
        );
    }

    public putFullRequest<T>(
        url: string, 
        payload: object
    ): Observable<HttpResponse<T>> {
        return this._http.put<T>(
            this.buildUrl(url),
            payload, 
            { 
                headers: this.getHeaders(), 
                observe: 'response' 
            }
        );
    }

    public deleteRequest<T>(
        url: string, 
        httpParams?: any
    ): Observable<T> {
        return this._http.delete<T>(
            this.buildUrl(url), 
            { 
                headers: this.getHeaders(), 
                params: httpParams 
            }
        );
    }

    public deleteFullRequest<T>(
        url: string, 
        httpParams?: any
    ): Observable<HttpResponse<T>> {
        return this._http.delete<T>(
            this.buildUrl(url), 
            { 
                headers: this.getHeaders(), 
                observe: 'response', 
                params: httpParams 
            }
        );
    }

    private buildUrl(url: string): string {
        if (
            url.startsWith('http://') || 
            url.startsWith('https://')
        ) {
            return url;
        }
        return this._baseUrl + url;
    }
}
