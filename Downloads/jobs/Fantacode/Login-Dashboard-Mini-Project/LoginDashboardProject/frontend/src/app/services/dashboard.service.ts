import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChartDataItem } from '../models/chart-data.model';
import { environment } from '../../environments/environment';
import { map } from 'rxjs/operators';
import { ChartData } from '../models/chart-data.model';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = `${environment.apiUrl}/dashboard`;

  constructor(private http: HttpClient) { }

  getChartData(): Observable<ChartDataItem[]> {
    return this.http.get<ChartDataItem[]>(`${this.apiUrl}/chart-data`);
  }

  // Map backend data to Chart.js format
  getFormattedChartData(): Observable<ChartData> {
    return this.getChartData().pipe(
      map(items => {
        const labels = items.map(item => item.label);
        const data = items.map(item => item.value);
        
        // Generate random colors for each data point
        const backgroundColor = items.map(() => this.getRandomColor(0.2));
        const borderColor = items.map(() => this.getRandomColor(1));
        
        return {
          labels,
          datasets: [{
            label: 'Ticket Status',
            data,
            backgroundColor,
            borderColor,
            borderWidth: 1
          }]
        };
      })
    );
  }

  private getRandomColor(opacity: number): string {
    const r = Math.floor(Math.random() * 255);
    const g = Math.floor(Math.random() * 255);
    const b = Math.floor(Math.random() * 255);
    return `rgba(${r}, ${g}, ${b}, ${opacity})`;
  }
}