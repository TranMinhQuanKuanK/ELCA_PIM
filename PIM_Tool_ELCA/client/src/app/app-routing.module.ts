import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddEditProjectComponent } from './add-edit-project/add-edit-project.component';
import { ErrorPageComponent } from './error-page/error-page.component';
import { ProjectListComponent } from './project-list/project-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'project', pathMatch: 'full' },
  { path: 'project', component: ProjectListComponent },
  { path: 'project/:id', component: AddEditProjectComponent, data: { mode: 'Edit' }, pathMatch: 'full' },
  { path: 'project/:id/', component: AddEditProjectComponent, data: { mode: 'Edit' }, pathMatch: 'full' },
  { path: 'new', component: AddEditProjectComponent, data: { mode: 'New' }, pathMatch: 'full' },
  { path: 'error-page', component: ErrorPageComponent, data: { message: 'Error Page' } },
  { path: '**', redirectTo: '/error-page' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
