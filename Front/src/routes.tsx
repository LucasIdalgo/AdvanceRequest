import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'

import { Login } from './Views/Login'
import { Home } from './Views/Home'
import { Contract } from './Views/Contract'
import { AdvanceRequest } from './Views/AdvanceRequest'
import { SignUp } from './Views/SignUp'
import { CreateContract } from './Views/CreateContract'
import { CreateAdvanceRequest } from './Views/CreateAdvanceRequest'

import { PrivateRoute } from './Service/PrivateRoute'

import { onLogin } from './Hooks/Clients/onLogin'
import { onRegister } from './Hooks/Clients/onRegister'
import { loadContracts } from './Hooks/Contracts/loadContracts'
import { loadAdvanceRequest } from './Hooks/AdvanceRequests/loadAdvancesRequests'
import { createdvanceRequest } from './Hooks/AdvanceRequests/createAdvanceRequests'
import { createContract } from './Hooks/Contracts/createContract'

export const AppRoutes = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/*' element={<Navigate to="/login" />} />

        <Route path='/login' element={<Login onLogin={onLogin} />} />

        <Route path='/signUp' element={<SignUp onRegister={onRegister} />} />
        
        <Route path='/home' element={
          <PrivateRoute>
            <Home />
          </PrivateRoute>} />

        <Route path='/contract' element={
          <PrivateRoute>
            <Contract loadContracts={loadContracts}/>
          </PrivateRoute>} />
          <Route path='/contract/create' element={
          <PrivateRoute>
            <CreateContract createContracts={createContract}/>
          </PrivateRoute>} />

        <Route path='/advanceRequest' element={
          <PrivateRoute>
            <AdvanceRequest loadAdvanceRequest={loadAdvanceRequest}/>
          </PrivateRoute>} />
          <Route path='/advanceRequest/create' element={
          <PrivateRoute>
            <CreateAdvanceRequest createAdvanceRequest={createdvanceRequest}/>
          </PrivateRoute>} />          
      </Routes>
    </BrowserRouter>
  )
}