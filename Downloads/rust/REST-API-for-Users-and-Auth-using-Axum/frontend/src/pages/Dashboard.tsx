import React, { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { api } from '../api/axios';
import { useNavigate } from 'react-router-dom';

interface User {
    id: string;
    name: string;
    email: string;
    role: string;
}

const Dashboard: React.FC = () => {
    const { user, logout } = useAuth();
    const [users, setUsers] = useState<User[]>([]);
    const navigate = useNavigate();

    const fetchUsers = async () => {
        try {
            const response = await api.get('/users');
            setUsers(response.data.data.users);
        } catch (err) {
            console.error('Failed to fetch users', err);
        }
    };

    useEffect(() => {
        fetchUsers();
    }, []);

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    const handleDelete = async (id: string) => {
        if (window.confirm('Are you sure you want to delete this user?')) {
            try {
                await api.delete(`/users/${id}`);
                setUsers(users.filter((u) => u.id !== id));
            } catch (err) {
                alert('Failed to delete user');
            }
        }
    };

    const handleEdit = async (id: string, currentName: string) => {
        const newName = prompt('Enter new name:', currentName);
        if (newName && newName !== currentName) {
            try {
                await api.put(`/users/${id}`, { name: newName });
                fetchUsers();
            } catch (err) {
                alert('Failed to update user');
            }
        }
    };

    return (
        <div className="dashboard-container">
            <header>
                <h1>Welcome, {user?.name}</h1>
                <button onClick={handleLogout} className="logout-btn">Logout</button>
            </header>

            <section>
                <h2>User Management</h2>
                <table className="user-table">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Email</th>
                            <th>Role</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((u) => (
                            <tr key={u.id}>
                                <td>{u.name}</td>
                                <td>{u.email}</td>
                                <td>{u.role}</td>
                                <td>
                                    <button onClick={() => handleEdit(u.id, u.name)}>Edit</button>
                                    <button onClick={() => handleDelete(u.id)} className="delete-btn">Delete</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </section>
        </div>
    );
};

export default Dashboard;
