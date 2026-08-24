import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UsuarioService } from '../../core/services/usuario.service';
import { Usuario } from '../../core/models/usuario.models';

type StatusFiltro = 'todos' | 'ativos' | 'inativos';

@Component({
  selector: 'app-usuario-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './usuario-list.component.html',
})
export class UsuarioListComponent {
  private readonly usuarioService = inject(UsuarioService);

  readonly usuarios = signal<Usuario[]>([]);
  readonly filtro = signal<StatusFiltro>('todos');
  readonly loading = signal(false);

  constructor() {
    this.carregar();
  }

  filtrarPor(filtro: StatusFiltro): void {
    this.filtro.set(filtro);
    this.carregar();
  }

  private carregar(): void {
    this.loading.set(true);
    const ativo = this.filtro() === 'todos' ? undefined : this.filtro() === 'ativos';

    this.usuarioService.list(ativo).subscribe({
      next: (usuarios) => {
        this.usuarios.set(usuarios);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }
}
