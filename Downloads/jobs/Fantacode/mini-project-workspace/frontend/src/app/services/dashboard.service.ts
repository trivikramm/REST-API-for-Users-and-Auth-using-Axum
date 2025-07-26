import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = 'http://localhost:5000/api';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  getChartData(): Observable<any> {
    return this.http.get(
      `${this.apiUrl}/dashboard/chart-data`,
      { headers: this.authService.getAuthHeaders() }
    );
  }
}
