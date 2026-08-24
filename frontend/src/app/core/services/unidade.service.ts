import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Unidade, UnidadeCreateRequest, UnidadeUpdateRequest } from '../models/unidade.models';

@Injectable({ providedIn: 'root' })
export class UnidadeService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/unidades`;

  list(): Observable<Unidade[]> {
    return this.http.get<Unidade[]>(this.baseUrl);
  }

  getById(id: number): Observable<Unidade> {
    return this.http.get<Unidade>(`${this.baseUrl}/${id}`);
  }

  create(dto: UnidadeCreateRequest): Observable<Unidade> {
    return this.http.post<Unidade>(this.baseUrl, dto);
  }

  updateStatus(id: number, dto: UnidadeUpdateRequest): Observable<Unidade> {
    return this.http.put<Unidade>(`${this.baseUrl}/${id}`, dto);
  }
}
