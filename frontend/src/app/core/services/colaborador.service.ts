import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  Colaborador,
  ColaboradorCreateRequest,
  ColaboradorUpdateRequest,
} from '../models/colaborador.models';

@Injectable({ providedIn: 'root' })
export class ColaboradorService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/colaboradores`;

  list(): Observable<Colaborador[]> {
    return this.http.get<Colaborador[]>(this.baseUrl);
  }

  getById(id: number): Observable<Colaborador> {
    return this.http.get<Colaborador>(`${this.baseUrl}/${id}`);
  }

  create(dto: ColaboradorCreateRequest): Observable<Colaborador> {
    return this.http.post<Colaborador>(this.baseUrl, dto);
  }

  update(id: number, dto: ColaboradorUpdateRequest): Observable<Colaborador> {
    return this.http.put<Colaborador>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
