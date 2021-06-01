import React, { useState, useEffect, useRef } from 'react';
import getAxios from '../AuthAxios';
import { HubConnectionBuilder } from '@microsoft/signalr';
import TaskRow from '../Components/TaskRow';
import { useUserContext } from '../UserContext';

const Home = () => {

    const [taskItems, setTaskItems] = useState([]);
    const [formTaskName, setformTaskName] = useState({ name: '' });
    const [disableButton, setDisableButton] = useState(false);
    const [rerender, setRerender] = useState(false);
    const { user } = useUserContext();
    const currentUser = user;
    const connectionRef = useRef(null);


    useEffect(() => {
        const getTaskItems = async () => {
            const { data } = await getAxios().get('/api/tasks/gettaskitems');
            setTaskItems(data);
        }
        const connectToHub = async () => {
            const connection = new HubConnectionBuilder().withUrl("/chat").build();
            await connection.start();
            connectionRef.current = connection;
            connection.on('newtaskadded', obj => {
                setTaskItems(obj);
            });
            connection.on('statusChanged', obj => {
               setRerender(true);
            });
        }
        getTaskItems();
        connectToHub();
        setRerender(false);
    }, [rerender])

    const onNameChange = e => {
        let form = {
            name: e.target.value
        };
        setformTaskName(form);
    }

    const onAddTaskClick = async () => {
        setDisableButton(true);
        await getAxios().post('/api/tasks/addtaskitem', formTaskName);
        setDisableButton(false);
        setformTaskName('');
    }

    


    return (
        <>
            <div className="row">
                <div className="col-md-10">
                    <input onChange={onNameChange} type="text" className="form-control" placeholder="Task Name" value={formTaskName.name} />
                </div>
                <div className="col-md-2">
                    <button disabled={disableButton} className="btn btn-primary btn-block" onClick={onAddTaskClick}>Add Task</button>
                </div>
            </div>
            <table className="table table-hover table-striped table-bordered mt-3">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {taskItems.map(t => <TaskRow
                        task={t}
                        key={t.id}
                     
                    />)}
                </tbody>
            </table >
        </>
    )
}
export default Home;