using Direct2Rep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Direct2Rep.DAL
{
    public class AdminDAL
    {
        Direct2RepEntities db = new Direct2RepEntities();
        #region User
        //This is reading function 
        public List<User> getUsersList()
        {
            List<User> users;


            users = db.Users.Where(x => x.IsActive == 1).ToList();


            return users;
        }

        public User getUserById(int _Id)
        {
            User user;


            user = db.Users.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);


            return user;
        }

        public bool AddUser(User _user)
        {


            db.Users.Add(_user);
            db.SaveChanges();

            return true;
        }

        public bool UpdateUser(User _user)
        {


            db.Entry(_user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public void DeleteUser(int _id)
        {


            db.Users.Remove(db.Users.FirstOrDefault(x => x.Id == _id));
            db.SaveChanges();

        }
        #endregion

        #region Company
        //This is reading function 
        public List<Company> getCompanysList()
        {
            List<Company> Companys;


            Companys = db.Companies.Where(x => x.IsActive == 1).ToList();


            return Companys;
        }

        public Company getCompanyById(int _Id)
        {
            Company Company;


            Company = db.Companies.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);


            return Company;
        }

        public bool AddCompany(Company _Company)
        {


            db.Companies.Add(_Company);
            db.SaveChanges();

            return true;
        }

        public bool UpdateCompany(Company _Company)
        {


            db.Entry(_Company).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public void DeleteCompany(int _id)
        {


            db.Companies.Remove(db.Companies.FirstOrDefault(x => x.Id == _id));
            db.SaveChanges();

        }
        #endregion

        #region SaleRepresentative
        //This is reading function 
        public List<SaleRepresentative> getSaleRepresentativesList()
        {
            List<SaleRepresentative> SaleRepresentatives;


            SaleRepresentatives = db.SaleRepresentatives.Where(x => x.IsActive == 1).ToList();


            return SaleRepresentatives;
        }

        public SaleRepresentative getSaleRepresentativeById(int _Id)
        {
            SaleRepresentative SaleRepresentative;


            SaleRepresentative = db.SaleRepresentatives.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);


            return SaleRepresentative;
        }

        public bool AddSaleRepresentative(SaleRepresentative _SaleRepresentative)
        {


            db.SaleRepresentatives.Add(_SaleRepresentative);
            db.SaveChanges();

            return true;
        }

        public bool UpdateSaleRepresentative(SaleRepresentative _SaleRepresentative)
        {


            db.Entry(_SaleRepresentative).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public void DeleteSaleRepresentative(int _id)
        {


            db.SaleRepresentatives.Remove(db.SaleRepresentatives.FirstOrDefault(x => x.Id == _id));
            db.SaveChanges();

        }
        #endregion

        #region Visitor
        //This is reading function 
        public List<Visitor> getVisitorsList()
        {
            List<Visitor> Visitors;


            Visitors = db.Visitors.Where(x => x.IsActive == 1).ToList();


            return Visitors;
        }

        public Visitor getVisitorById(int _Id)
        {
            Visitor Visitor;


            Visitor = db.Visitors.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);


            return Visitor;
        }

        public bool AddVisitor(Visitor _Visitor)
        {


            db.Visitors.Add(_Visitor);
            db.SaveChanges();

            return true;
        }

        public bool UpdateVisitor(Visitor _Visitor)
        {


            db.Entry(_Visitor).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public void DeleteVisitor(int _id)
        {


            db.Visitors.Remove(db.Visitors.FirstOrDefault(x => x.Id == _id));
            db.SaveChanges();

        }
        #endregion

        #region Company_SaleRep
        //This is reading function 
        public List<Company_SaleRep> getCompany_SaleRepsList()
        {
            List<Company_SaleRep> company_SaleReps;


            company_SaleReps = db.Company_SaleRep.Where(x => x.IsActive == 1).ToList();


            return company_SaleReps;
        }

        public Company_SaleRep getCompany_SaleRepById(int _Id)
        {
            Company_SaleRep Company_SaleRep;


            Company_SaleRep = db.Company_SaleRep.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);


            return Company_SaleRep;
        }

        public bool AddCompany_SaleRep(Company_SaleRep _Company_SaleRep)
        {


            db.Company_SaleRep.Add(_Company_SaleRep);
            db.SaveChanges();

            return true;
        }

        public bool UpdateCompany_SaleRep(Company_SaleRep _Company_SaleRep)
        {


            db.Entry(_Company_SaleRep).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public void DeleteCompany_SaleRep(int _id)
        {


            db.Company_SaleRep.Remove(db.Company_SaleRep.FirstOrDefault(x => x.Id == _id));
            db.SaveChanges();

        }
        #endregion

        #region SaleRep_States
        //This is reading function 
        public List<State> getStateList()
        {
            List<State> company_SaleReps;
            company_SaleReps = db.States.Where(x => x.IsActive == 1).ToList();
            return company_SaleReps;
        }

        public State getStateById(int _Id)
        {
            State Company_SaleRep;
            Company_SaleRep = db.States.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);
            return Company_SaleRep;
        }

        public bool AddState(State _Company_SaleRep)
        {
            db.States.Add(_Company_SaleRep);
            db.SaveChanges();

            return true;
        }

        public bool UpdateState(State _Company_SaleRep)
        {
            db.Entry(_Company_SaleRep).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public void DeleteState(int _id)
        {
            db.States.Remove(db.States.FirstOrDefault(x => x.Id == _id));
            db.SaveChanges();

        }
        #endregion

        #region Visitor_States
        //This is reading function 
        public List<Visitor_States> getVisitor_StatesList()
        {
            List<Visitor_States> company_SaleReps;
            company_SaleReps = db.Visitor_States.Where(x => x.IsActive == 1).ToList();
            return company_SaleReps;
        }

        public Visitor_States getVisitor_StatesById(int _Id)
        {
            Visitor_States Company_SaleRep;
            Company_SaleRep = db.Visitor_States.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);
            return Company_SaleRep;
        }

        public bool AddVisitor_States(Visitor_States _Company_SaleRep)
        {
            db.Visitor_States.Add(_Company_SaleRep);
            db.SaveChanges();

            return true;
        }

        public bool UpdateVisitor_States(Visitor_States _Company_SaleRep)
        {
            db.Entry(_Company_SaleRep).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public void DeleteVisitor_States(int _id)
        {
            db.Visitor_States.Remove(db.Visitor_States.FirstOrDefault(x => x.Id == _id));
            db.SaveChanges();

        }
        #endregion

        #region RepStatePart
        //This is reading function 
        public List<RepStatePart> getRepStatePartList()
        {
            List<RepStatePart> company_SaleReps;
            company_SaleReps = db.RepStateParts.Where(x => x.IsActive == 1).ToList();
            return company_SaleReps;
        }

        public RepStatePart getRepStatePartById(int _Id)
        {
            RepStatePart Company_SaleRep;
            Company_SaleRep = db.RepStateParts.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);
            return Company_SaleRep;
        }

        public bool AddRepStatePart(RepStatePart _Company_SaleRep)
        {
            db.RepStateParts.Add(_Company_SaleRep);
            db.SaveChanges();

            return true;
        }

        public bool UpdateRepStatePart(RepStatePart _Company_SaleRep)
        {
            db.Entry(_Company_SaleRep).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public void DeleteRepStatePart(int _id)
        {
            db.RepStateParts.Remove(db.RepStateParts.FirstOrDefault(x => x.Id == _id));
            db.SaveChanges();
        }
        #endregion

        #region Campaign

        public List<Campaign> getCampaignList()
        {
            List<Campaign> company_SaleReps;
            company_SaleReps = db.Campaigns.Where(x => x.IsActive == 1).ToList();
            return company_SaleReps;
        }

        public Campaign getCampaignById(int _Id)
        {
            Campaign Company_SaleRep;
            Company_SaleRep = db.Campaigns.Where(x => x.IsActive == 1).FirstOrDefault(x => x.Id == _Id);
            return Company_SaleRep;
        }

        public bool AddCampaign(Campaign _Company_SaleRep)
        {
            db.Campaigns.Add(_Company_SaleRep);
            db.SaveChanges();

            return true;
        }

        public bool UpdateCampaign(Campaign _Company_SaleRep)
        {
            db.Entry(_Company_SaleRep).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public void DeleteCampaign(int _id)
        {
            db.Campaigns.Remove(db.Campaigns.FirstOrDefault(x => x.Id == _id));
            db.SaveChanges();
        }
        #endregion
    }
}