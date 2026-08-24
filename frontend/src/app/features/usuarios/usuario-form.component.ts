import { Component, computed, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { UsuarioService } from '../../core/services/usuario.service';

@Component({
  selector: 'app-usuario-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './usuario-form.component.html',
})
export class UsuarioFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly usuarioService = inject(UsuarioService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly idParam = this.route.snapshot.paramMap.get('id');
  readonly usuarioId = this.idParam ? Number(this.idParam) : null;
  readonly isEdit = computed(() => this.usuarioId !== null);

  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly loginAtual = signal<string | null>(null);

  readonly createForm = this.fb.group({
    codigo: ['', Validators.required],
    login: ['', Validators.required],
    senha: ['', Validators.required],
  });

  readonly editForm = this.fb.group({
    senha: [''],
    ativo: [true],
  });

  constructor() {
    if (this.usuarioId !== null) {
      this.usuarioService.getById(this.usuarioId).subscribe((usuario) => {
        this.loginAtual.set(usuario.login);
        this.editForm.patchValue({ ativo: usuario.ativo });
      });
    }
  }

  submitCreate(): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set(null);

    this.usuarioService.create(this.createForm.getRawValue() as {
      codigo: string;
      login: string;
      senha: string;
    }).subscribe({
      next: () => this.router.navigate(['/usuarios']),
      error: (err) => {
        this.errorMessage.set(err?.error?.detail ?? 'Não foi possível cadastrar o usuário.');
        this.loading.set(false);
      },
    });
  }

  submitEdit(): void {
    if (this.usuarioId === null) {
      return;
    }

    this.loading.set(true);
    this.errorMessage.set(null);

    const raw = this.editForm.getRawValue();
    this.usuarioService.update(this.usuarioId, {
      senha: raw.senha ? raw.senha : null,
      ativo: raw.ativo,
    }).subscribe({
      next: () => this.router.navigate(['/usuarios']),
      error: (err) => {
        this.errorMessage.set(err?.error?.detail ?? 'Não foi possível atualizar o usuário.');
        this.loading.set(false);
      },
    });
  }
}
