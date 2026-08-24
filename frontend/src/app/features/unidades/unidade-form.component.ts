import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UnidadeService } from '../../core/services/unidade.service';

@Component({
  selector: 'app-unidade-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './unidade-form.component.html',
})
export class UnidadeFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly unidadeService = inject(UnidadeService);
  private readonly router = inject(Router);

  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly form = this.fb.group({
    codigo: ['', Validators.required],
    nome: ['', Validators.required],
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set(null);

    this.unidadeService.create(this.form.getRawValue() as { codigo: string; nome: string }).subscribe({
      next: () => this.router.navigate(['/unidades']),
      error: (err) => {
        this.errorMessage.set(err?.error?.detail ?? 'Não foi possível cadastrar a unidade.');
        this.loading.set(false);
      },
    });
  }
}
