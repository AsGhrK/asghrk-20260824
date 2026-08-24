import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest, TokenResponse } from '../models/auth.models';

const TOKEN_KEY = 'gestao_colaboradores_token';
const EXPIRES_KEY = 'gestao_colaboradores_token_expires_at';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly isAuthenticatedSignal = signal(this.hasValidToken());
  readonly isAuthenticated = this.isAuthenticatedSignal.asReadonly();

  constructor(private readonly http: HttpClient) {}

  login(credentials: LoginRequest): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(`${environment.apiUrl}/auth/login`, credentials).pipe(
      tap((response) => {
        localStorage.setItem(TOKEN_KEY, response.token);
        localStorage.setItem(EXPIRES_KEY, response.expiresAt);
        this.isAuthenticatedSignal.set(true);
      }),
    );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(EXPIRES_KEY);
    this.isAuthenticatedSignal.set(false);
  }

  getToken(): string | null {
    return this.hasValidToken() ? localStorage.getItem(TOKEN_KEY) : null;
  }

  private hasValidToken(): boolean {
    const token = localStorage.getItem(TOKEN_KEY);
    const expiresAt = localStorage.getItem(EXPIRES_KEY);
    if (!token || !expiresAt) {
      return false;
    }
    return new Date(expiresAt).getTime() > Date.now();
  }
}
