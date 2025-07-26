import { Component, OnDestroy, OnInit } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { DashboardService } from '../services/dashboard.service';
import { Subscription } from 'rxjs';

Chart.register(...registerables);

// It's a good practice to define an interface for your data structures.
interface ChartData {
  label: string;
  value: number;
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {
  chart?: Chart;
  private dataSubscription: Subscription | undefined;

  constructor(
    private dashboardService: DashboardService
  ) { }

  ngOnInit() {
    this.fetchChartData();
  }

  ngOnDestroy(): void {
    this.dataSubscription?.unsubscribe();
    this.chart?.destroy();
  }

  fetchChartData() {
    this.dataSubscription = this.dashboardService.getChartData()
      .subscribe({
        next: (data: ChartData[]) => {
          const labels = data.map(item => item.label);
          const values = data.map(item => item.value);
          
          this.createChart(labels, values);
        },
        error: (err) => console.error('Failed to get chart data', err)
    });
  }

  private createChart(labels: string[], values: number[]) {
    // If a chart instance already exists, destroy it before creating a new one.
    this.chart?.destroy();

    this.chart = new Chart('myChart', {
      type: 'bar',
      data: {
        labels: labels,
        datasets: [{
          label: 'Ticket Status',
          data: values,
          backgroundColor: [
            'rgba(255, 99, 132, 0.5)',
            'rgba(54, 162, 235, 0.5)',
            'rgba(255, 206, 86, 0.5)',
            'rgba(75, 192, 192, 0.5)'
          ],
          borderColor: [
            'rgba(255, 99, 132, 1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)'
          ],
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    });
  }
}